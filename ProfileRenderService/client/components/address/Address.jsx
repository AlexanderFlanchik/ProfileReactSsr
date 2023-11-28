const React = require('react');

function Address(props) {
    const data = props.data;

    return (
    <div className="address-container">
        <div className='address-property-container'>
            <div className="address-title">City:</div>
            <div className='address-city'>{data.city ? data.city : "No data"}</div>
        </div>
        {data.street && (<div className='address-property-container'>
            <div className="address-title">Street:</div>
            <div className='address-street'>{data.street}</div>
        </div>)}
    </div>
    );
}

module.exports = Address;