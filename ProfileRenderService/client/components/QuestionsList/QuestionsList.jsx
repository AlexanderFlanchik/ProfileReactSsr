const React = require('react');
const toDate = require('../../../services/getDate');

function QuestionsList(props) {
    const { latestQuestions } = props;

    const Question = (data) => {
        const question = data.question;
        const timestampText = toDate(question.timestamp.seconds);

        return (
            <div className='question-item'>
                <div className='question-item-title'>
                    <div>{timestampText}</div>
                    <div>{question.customerName}</div>
                </div>
                <div className='question-item-body'>
                    <a href="#">{question.text}</a>
                </div>
            </div>
        );
    };

    const renderQuestions = () => {
        if (latestQuestions && latestQuestions.length) {
            return (
                <div className="questions-list">
                    {latestQuestions.map(q => (<Question question={q} key={q.questionId} />))}
                </div>
            );
        } else {
            return (
                <div className='no-questions-msg'>
                    No questions yet..
                </div>
            );
        }
    };

    return (
        <div className='questions-list'>
            <div className='questions-list-title'>Recent Questions</div>
            {renderQuestions()}
        </div>
    );
}

module.exports = QuestionsList;