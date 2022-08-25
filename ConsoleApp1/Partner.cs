namespace EF_DDD
{
    public class Partner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        protected Partner(){}
        internal Partner(string name):this()
        {
            Name = name;
        }
    }
}
