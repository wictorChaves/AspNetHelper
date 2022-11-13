namespace SwaggerGetComment
{
    public class WeatherForecast
    {
        /// <summary>
        /// É a data né
        /// </summary>
        /// <example>2022-02-02</example>
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public string? Summary { get; set; }
    }
}