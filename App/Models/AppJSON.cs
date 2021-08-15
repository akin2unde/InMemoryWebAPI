namespace InMemoryWebAPI.Models{
    class AppJSON :IAppJSON {
        public int AbsoluteExpiration { get; set; }=5;
        public int SlidingExpiration { get; set; }=5;
        public int Size { get; set; } =1024;
        public string MemoryName { get; set; }="UserList";
    }

    public interface IAppJSON {
         int AbsoluteExpiration { get; set; } 
         int SlidingExpiration { get; set; } 
         int Size { get; set; } 
         string MemoryName { get; set; } 
    }
}

