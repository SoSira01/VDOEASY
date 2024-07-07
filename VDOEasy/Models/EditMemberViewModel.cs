using VDOEasy.Models;

namespace VDOEasy.ViewModels
{
    public class EditMemberViewModel
    {
        public TrnMembers Member { get; set; }
        public List<TrnMembersMovieType> SelectMovieTypes { get; set; }
        public List<MasMovieType> MasMovieTypes { get; set; }
    }
}
