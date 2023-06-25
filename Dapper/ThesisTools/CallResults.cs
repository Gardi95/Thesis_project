using DapperAPI.Models;

namespace DapperAPI.ThesisTools
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
