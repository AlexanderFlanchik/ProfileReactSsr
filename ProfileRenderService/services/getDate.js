const toDate = (seconds) => {
    const d = new Date(Date.UTC(1970, 0, 1));
    d.setUTCSeconds(seconds);
    return d.toLocaleDateString();
};

module.exports = toDate;
