namespace API.Controllers;

[Authorize]
public class MessagesController : BaseApiController
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public MessagesController(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
	{
		messageParams.Username = User.GetUsername();

		var messages = await _unitOfWork.MessageRepository.GetMessagesForUser(messageParams);

		Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

		return messages;
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteMessage(int id)
	{
		var username = User.GetUsername();
		var message = await _unitOfWork.MessageRepository.GetMessage(id);

		//Check if trying to delete a message that is not ours
		if (message.Sender.UserName != username && message.Recipient.UserName != username) return Unauthorized();

		//Flag deleted on user side
		if (message.Sender.UserName == username) message.SenderDeleted = true;
		if (message.Recipient.UserName == username) message.RecipientDeleted = true;

		//Delete message
		if (message.SenderDeleted && message.RecipientDeleted) _unitOfWork.MessageRepository.DeleteMessage(message);

		//Save
		if (await _unitOfWork.Complete()) return Ok();

		return BadRequest("Problem deleting the message");
	}
}