using System.Globalization;

namespace AdventOfCode.Y2021.Day16;

public class PacketDecoder : AocSolution<List<bool>> {
    public override string Name => "Packet Decoder";

    protected override List<bool> ProcessInput(string input) {
        string text = input.Trim();
        List<bool> bits = new();
        foreach (char c in text) {
            byte b = byte.Parse(c.ToString(), NumberStyles.HexNumber);
            for (int i = 0; i < 4; i++) {
                bits.Add((b & 0x8) > 0);
                b <<= 1;
            }
        }

        return bits;
    }

    protected override string Part1Implementation(List<bool> bits) {
        int ReadTotalVersionNumbers(Packet packet) {
            int total = packet.Version;
            if (packet is Operation op) {
                total += op.Children.Select(x => ReadTotalVersionNumbers(x)).Sum();
            }

            return total;
        }

        Packet packet = ReadPacket(new BitReader(bits));
        int totalVersionNumbers = ReadTotalVersionNumbers(packet);

        return $"Sum of packet versions: {totalVersionNumbers}";
    }

    protected override string Part2Implementation(List<bool> input) {
        long CalculatePacket(Packet packet) {
            if (packet is Literal literal) {
                return literal.Value;
            }

            if (packet is Operation op) {
                var calculatedChildren = op.Children.Select(x => CalculatePacket(x)).ToArray();
                return op.Type switch
                {
                    // Sum all children
                    0 => calculatedChildren.Sum(),
                    // Multiply all children
                    1 => calculatedChildren.Aggregate(1L, (i, x) => i * x),
                    // Get minimum value
                    2 => calculatedChildren.Min(),
                    // Get max value
                    3 => calculatedChildren.Max(),

                    // Greater than 
                    5 => calculatedChildren[0] > calculatedChildren[1] ? 1 : 0,
                    // Lesser than 
                    6 => calculatedChildren[0] < calculatedChildren[1] ? 1 : 0,
                    // Equal
                    7 => calculatedChildren[0] == calculatedChildren[1] ? 1 : 0,

                    // Throw
                    _ => throw new Exception("Unknown op type")
                };
            }

            throw new Exception("Unknown packet");
        }

        Packet packet = ReadPacket(new BitReader(input));
        long packetValue = CalculatePacket(packet);

        return $"Result of packet calculation: {packetValue}";
    }

    private class BitReader {
        public int Index { get; private set; }
        private readonly List<bool> bits;

        public BitReader(List<bool> bits) {
            Index = 0;
            this.bits = bits;
        }

        public int ReadInt(int bitCount) {
            int number = 0;
            for (int i = 0; i < bitCount; i++) {
                number <<= 1;
                number |= bits[Index++] ? 1 : 0;
            }

            return number;
        }
    }

    private abstract class Packet {
        public readonly int Version;
        public readonly int Type;

        protected Packet(int version, int type) {
            Version = version;
            Type = type;
        }
    }

    private class Operation : Packet {
        public Packet[] Children;

        public Operation(int version, int type, Packet[] children) : base(version, type) {
            Children = children;
        }
    }

    private class Literal : Packet {
        public readonly long Value;

        public Literal(int version, int type, long value) : base(version, type) {
            Value = value;
        }
    }

    private static Packet ReadPacket(BitReader reader) {
        int version = reader.ReadInt(3);
        int type = reader.ReadInt(3);

        // Literal packet
        if (type == 4) {
            long literalValue = 0;

            while (true) {
                bool isFinal = reader.ReadInt(1) == 0;
                literalValue <<= 4;
                literalValue |= reader.ReadInt(4);
                if (isFinal) {
                    break;
                }
            }

            return new Literal(version, type, literalValue);
        }

        int lengthTypeId = reader.ReadInt(1);
        List<Packet> packets = new();

        if (lengthTypeId == 0) {
            int totalLengthInBits = reader.ReadInt(15);
            int endIndex = reader.Index + totalLengthInBits;
            while (reader.Index < endIndex) {
                packets.Add(ReadPacket(reader));
            }
        }
        else {
            int subPacketCount = reader.ReadInt(11);
            for (int i = 0; i < subPacketCount; i++) {
                packets.Add(ReadPacket(reader));
            }
        }

        return new Operation(version, type, packets.ToArray());
    }
}