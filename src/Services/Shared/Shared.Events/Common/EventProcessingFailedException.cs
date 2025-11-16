using Shared.Base.Errors;

namespace Shared.Events.Common;

public sealed class EventProcessingFailedException(ErrorResult errorResult) : Exception(errorResult.ErrorCode);