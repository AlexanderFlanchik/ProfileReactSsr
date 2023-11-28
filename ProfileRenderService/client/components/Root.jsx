const React = require("react");
const assets = require('./../../dist/static/manifest.json');

const App = require('./App');

function Root(initialProps) {
    const linkUrl = `/content/${assets["client.css"]}`;
    const scriptUrl = `/content/${assets["client.js"]}`;

    const initialPropsData = JSON.stringify(initialProps);
    const initialStateScript = `window.__INITIAL_STATE = ${initialPropsData}`;

    return (
        <html>
            <head>
                <meta charSet="utf-8" />
                <title>Profile</title>
                <link rel="stylesheet" href={linkUrl} type="text/css" />
                <script type="text/javascript" dangerouslySetInnerHTML={{__html: initialStateScript}}></script>
            </head>
            <body>
                <App {...initialProps} />
                <script type="text/javascript" src={scriptUrl}></script>
            </body>
        </html>
    );
}

module.exports = Root;
