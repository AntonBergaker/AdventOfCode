#[macro_export]
macro_rules! highlight_part {
    ($template_string:expr, $value:expr) => {
        format!($template_string, colored::Colorize::yellow( format!("{}", $value).as_str() ))
    }
}
