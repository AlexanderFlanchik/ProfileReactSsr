const React = require('react');

function Footer() {
    return (
        <div className="footer">
            Copyright &copy; {new Date().getFullYear()} | <a href="#">Contact us</a>
        </div>);
}

module.exports = Footer;
