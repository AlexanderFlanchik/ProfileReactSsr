namespace ProfileGateway.Models
{
    public class ProfileInfo
    {
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string GeneralInfo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public string PreferredLanguage { get; set; }
        public AddressDto Address { get; set; }
        public IList<string> Categories { get; set; }
        public IList<FeedbackData> Feedbacks { get; set; }
        public IList<QuestionData> LatestQuestions { get; set; }
    }
}