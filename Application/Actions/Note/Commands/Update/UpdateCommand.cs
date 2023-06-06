using MediatR;

namespace Application.Actions.Note.Commands.Update
{
    public class UpdateCommand:IRequest
    {
        public Guid NoteId { get; set; }
        public Guid UserId { get; set; }


        public bool? Share { get; set; } = null;
        public bool? Check { get; set; } = null;
        public string? Title { get; set; } = null;
        public string? Description { get; set; } = null;
        public Guid? Parent { get; set; } = null;
        public Guid? User { get; set; } = null;
        public double? Weight { get; set; } = null;
        public int? Number { get; set; } = null;
    }
}
