using Grpc.Core;
using GoogleTimestamp = Google.Protobuf.WellKnownTypes.Timestamp;

namespace ProfileDataService.Services
{
    public class ProfileInfoService : ProfileDataService.ProfileDataServiceBase
    {
        private readonly IProfileProvider _profileProvider;

        public ProfileInfoService(IProfileProvider profileProvider)
        {
            _profileProvider = profileProvider;
        }

        public override async Task<ProfileResponse> GetProfileData(ProfileRequest request, ServerCallContext context)
        {
            var profileInfo = await _profileProvider.GetProfileAsync(request.UserId);

            var profileResponse = new ProfileResponse
            {
                AvatarUrl = profileInfo.AvatarUrl,
                UserName = profileInfo.Name,
                UserGeneralInfo = profileInfo.GeneralInfo,
                UserAge = profileInfo.Age,
                EmailAddress = profileInfo.Email,
                PhoneNumber = profileInfo.Phone,
                Zip = profileInfo.ZipCode,
                PreferredLanguage = profileInfo.PreferredLanguage,
                AddressData = new AddressData { Street = profileInfo.Address.Street, City = profileInfo.Address.City },
            };

            foreach (var category in profileInfo.Categories) 
            {
                profileResponse.Categories.Add(category);
            }

            foreach (var feedback in profileInfo.Feedbacks)
            {
                profileResponse.Feedbacks.Add(
                    new Feedback 
                    { 
                        CustomerName = feedback.CustomerName, 
                        FeedbackBody = feedback.FeedbackBody, 
                        Timestamp = GoogleTimestamp.FromDateTime(feedback.Timestamp.ToUniversalTime()) 
                    });
            }

            foreach (var question in profileInfo.LatestQuestions)
            {
                profileResponse.LatestQuestions.Add(
                    new Question
                    {
                        QuestionId = question.QuesitionId,
                        CustomerName = question.CustomerName,
                        Text = question.Text,
                        Timestamp = GoogleTimestamp.FromDateTime(question.Timestamp.ToUniversalTime())
                    });
            }

            return profileResponse;
        }
    }
}