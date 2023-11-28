const React = require('react');
const { hydrateRoot } = require('react-dom/client');
const Root = require('./components/Root.jsx');

import '../style.less';

hydrateRoot(document, <Root {...window.__INITIAL_STATE} />);