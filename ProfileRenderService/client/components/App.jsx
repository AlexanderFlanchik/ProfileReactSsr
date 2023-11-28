const React = require("react");
const Profile = require("./profile/Profile.jsx");
const Footer = require('./footer/Footer.jsx');

function App (props) {

    return (
        <div className="app-wrapper">
           <div className="header-container">
                <h1>Expert Profile</h1> 
           </div>
           <Profile {...props} />
           <Footer />
        </div>
    );
};

module.exports = App;
