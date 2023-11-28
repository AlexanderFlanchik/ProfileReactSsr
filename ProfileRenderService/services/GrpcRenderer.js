const getHtmlResponse = require('./HtmlRenderer.js');

module.exports = {
    renderContent: (call, callback) => {
        
        const request = call.request;

        const data = {
            avatarUrl: request.avatarUrl,
            name: request.name,
            age: request.age,
            generalInfo: request.generalInfo,
            email: request.email,
            phone: request.phone,
            preferredLanguage: request.preferredLanguage,
            zipCode: request.zipCode,
            address: request.address ? {
                city: request.address.city,
                street: request.address.street
            } : null,
            categories: request.categories,
            feedbacks: request.feedbacks
                        .map(f => (
                            { 
                                customerName : f.customer_name, 
                                feedbackBody : f.feedback_body, 
                                timestamp: f.timestamp
                            })),
            latestQuestions: request.latest_questions
                        .map(q => (
                            { 
                                questionId : q.question_id, 
                                customerName : q.customer_name, 
                                text : q.text, 
                                timestamp : q.timestamp
                            }))
        };

        const htmlContent = getHtmlResponse(data);
        callback(null, {
            html: htmlContent,
            status: 200
        });
    }
}