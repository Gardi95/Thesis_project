using EntityFramework.Models;

namespace EntityFramework.ThesisTools
{
    public class CallResults
    {
        public Stats stats { get; set; }
        public List<Books> data { get; set; }       
        
        public CallResults()
        {
            stats = new Stats();
            data = new List<Books>();
        }
    }
}
