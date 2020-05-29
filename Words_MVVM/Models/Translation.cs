namespace Words_MVVM.Models
{
    public class Translation
    {
        public string Text { get; set; }

        public string Pos { get; set; }

        public Synonym[] Syn { get; set; }

        public Meaning[] Mean { get; set; }

        public Example[] Ex { get; set; }
    }
}
