namespace GymManagement.Shared.Core.BaseObjects
{
    public class BaseObject
    {
        public int Id { get; }

        public DateTime CreatedAt { get; }

        public DateTime? UpdatedAt { get; }

        protected BaseObject(int id, DateTime createdAt, DateTime? updatedAt = null)
        {
            Id = id;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
