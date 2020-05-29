namespace Words_MVVM.Models
{
    public class Definition
    {
        public string Text { get; set; }

        public string Pos { get; set; }

        public Translation[] Tr { get; set; }
    }
}
