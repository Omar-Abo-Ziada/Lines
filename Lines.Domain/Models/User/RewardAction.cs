using Lines.Domain.Enums;
using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.User
{
    public class RewardAction : BaseModel
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public RewardActionType Type { get; set; }

        public RewardAction(string name, int points, RewardActionType type)
        {
            Name = name;
            Points = points;
            Type = type;
        }

        // Parameterless constructor for EF
        public RewardAction() { }
    }
}
