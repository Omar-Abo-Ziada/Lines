namespace Lines.Application.Features.Feedbacks
{
    public static class FeedbackErrors
        {
            public static Error SendFeedbackError(string desc) => new Error("Feedback.Send_Feedback_ERROR", desc, ErrorType.Validation);

        }
}
