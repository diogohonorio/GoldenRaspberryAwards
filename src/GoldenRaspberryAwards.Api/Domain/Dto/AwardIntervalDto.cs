namespace GoldenRaspberryAwards.Api.Domain.Dto
{
    public class AwardIntervalDto
    {
        public string Producer { get; set; } = null!;
        public int Interval { get; set; }
        public int PreviousWin { get; set; }
        public int FollowingWin { get; set; }
    }

}
