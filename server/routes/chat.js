var express = require('express');
var router = express.Router();
var fs = require('fs');
var chat = require('./chat.json');


/* GET home page. */
router.get('/get', function (req, res, next) {
    

    res.send(chat);
});

router.get('/set', function (req, res, next) {
    let username = req.query.username;
    let messageText = req.query.messageText;

    let message = JSON.parse(username, messageText);

    chat.messages.push(message);

    res.render('index', { title: 'Get Chat' });
});



function isNotWord(message) {
    let messageArr = message.split(/[ ]+/);
    console.log(messageArr);
    let cleanMessageArr = messageArr.map(m => m.replace(/[^\W\s]/gi, ''));

    let testobj = {'time':'2020', 'text':'mitt fina meddelande att skicka'};
    fs.appendFile('chat.json', ',\n' + JSON.stringify(testobj), function upd() {
        console.log("TODO error if error");
    });
}

module.exports = router;