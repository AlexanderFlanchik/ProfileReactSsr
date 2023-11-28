const express = require('express');
const getHtmlResponse = require('../services/HtmlRenderer.js');

const router = express.Router();

router.get('/profile', (req, res) => {

    let data = null;

    try {
        const query = req.query ? req.query["data"]?.toString() : "";
        
        if (query) {
            data = JSON.parse(query);
        } else {
            return res.status(400).end();
        }
    } catch {
        return res.status(400).end();
    }

    res.header("content-type", "text/html");
    res.send(getHtmlResponse(data));
});

router.post('/profile', (req, res) => {
    
    if (!req.body) {
        return res.status(400).end();
    }

    const data = req.body;
    
    const htmlContent = getHtmlResponse(data);
    const response = {
        html: htmlContent,
        status: 200
    }
    
    res.json(response);
});

module.exports =  router;
