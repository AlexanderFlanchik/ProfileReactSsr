const React = require('react');
const Root = require('./../client/components/Root.jsx');
const ReactDOMServer = require('react-dom/server');

const getHtmlResponse = (data)  => {
    const htmlContent = ReactDOMServer.renderToString(React.createElement(Root, data));
   
    return htmlContent;
};

module.exports = getHtmlResponse;
