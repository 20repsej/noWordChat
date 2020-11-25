var express = require('express');
var router = express.Router();
var bodyParser = require('body-parser')
var fs = require('fs');
var chat = require('./chat.json');

// Use bodyParser for POST requests
router.use(bodyParser.urlencoded({ extended: false }))

// Receive POST request from client
router.post('/post', function (req, res){

    let json = req.body;
    console.log(json);

    let username = json.messages.username;
    let message = json.messages.message;

    chat.messages.push(json.messages[0].username);

    fs.writeFile("chat.json", chat, x => x);

    console.log("Username: " + username);

    res.send();
});

// Send old messages to client
router.get('/getFirst', function (req, res, next) {


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

    let testobj = { 'time': '2020', 'text': 'mitt fina meddelande att skicka' };
    fs.appendFile('chat.json', ',\n' + JSON.stringify(testobj), function upd() {
        console.log("TODO error if error");
    });
}

module.exports = router;