const React = require('react');
const toDate = require('../../../services/getDate');

function Feedbacks(props) {
    const { feedbacks } = props;

    const Feedback = (data) => {
        const feedback = data.feedback;
        const timestampText = toDate(feedback.timestamp.seconds);

        return (
            <div className='feedback-item'>
                <div className='feedback-item-title'>
                    <div>{timestampText}</div>
                    <div>{feedback.customerName}</div>
                </div>
                <div className='feedback-item-body'>
                    {feedback.feedbackBody}
                </div>
            </div>
        );
    };

    const renderFeedbacks = ()=> {
        if (feedbacks && feedbacks.length) {
            return (
                <div className="feedbacks-list">
                    {feedbacks.map(f => (<Feedback feedback={f} key={f.timestamp} />))}
                </div>
            );
        } else {
            return (
                <div className='no-feedbacks-msg'>
                    No customer feedbacks for now.
                </div>
            );
        }
    };

    return (
        <div className="feedbacks-container">
            <div className="feedbacks-container-title">Feedbacks from customers:</div>
            {renderFeedbacks()}
        </div>
    );
}

module.exports = Feedbacks;
