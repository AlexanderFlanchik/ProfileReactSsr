const React = require("react");
const Address = require("./../address/Address.jsx");
const Feedbacks = require('../feedbacks/Feedbacks.jsx');
const QuestionsList = require('../QuestionsList/QuestionsList.jsx');

function Profile (props)  {
    const ageData = `Age: ${props.age} years old.`;
    const zip = props.zipCode ? props.zipCode : "No data";
    const prefLanguage = props.preferredLanguage ? props.preferredLanguage : "No data";
    const address = props.address;

    const addressContainer = !!address ? (<Address data={address} />) : (<div className="profile-no-address">No address info provided.</div>);
    const categories = (props.categories || []).join(', ');

    return (
        <div className="profile-outer-container">
            <div className="profile">
                <div className="profile-info-container">
                    <div className="profile-image-container">
                        <img src={props.avatarUrl} />
                    </div>
                    <div className="profile-container">
                        <div className="profile-name">
                            {props.name}
                        </div>
                        <div className="profile-age">
                            {ageData}
                        </div>
                    </div>
                </div>
                <div className="profile-details-container">
                    <div className="profile-details-header">Details:</div>
                    <div className="profile-details">
                        {props.generalInfo}
                    </div>
                </div>
                <div className="profile-language">
                    <span>Preferred language:</span> {prefLanguage}
                </div>
                <div className="profile-email-and-phone-container">
                    <div className="email">
                        <span>Email: </span>
                        {props.email}
                    </div>
                    <div className="phone">
                        <span>Phone: </span>
                        {props.phone}
                    </div>
                </div>
                <div className="profile-zip">
                    <span>Zip code: </span> {zip}
                </div>
                {addressContainer}
                <div className="categories">
                    <span>Categories:</span> {categories}
                </div>
                <Feedbacks feedbacks={props.feedbacks} />
            </div>
            <div className="questions-list-container">
                <QuestionsList latestQuestions={props.latestQuestions} />
            </div>
        </div>
    );
}

module.exports = Profile;
