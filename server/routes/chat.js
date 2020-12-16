var express = require('express');
var router = express.Router();
var bodyParser = require('body-parser')
var fs = require('fs');
const { json } = require('body-parser');

// Use bodyParser for POST requests
router.use(bodyParser.urlencoded({ extended: false }));
router.use(bodyParser.json());

// Restore saved chat log
let chat = JSON.parse(fs.readFileSync('./chat.json'));
console.log(chat);

// Receive POST request with new message from client
router.post('/post', function (req, res) {

    // expected format: { username: '20repsej', messageText: 'This is my message' }
    let message = req.body;

    // log recieved data for debugging
    console.log("Username: " + message.username);
    console.log("Message text: " + message.messageText);

    // Add time to message
    let time = Date.now();
    message.time = time;

    // Save new chatlog as text file
    console.log(chat.messages);
    chat.messages.push(message);
    console.log(chat);
    newChatFile = JSON.stringify(chat);
    console.log(newChatFile);
    try {
        fs.writeFile("./chat.json", newChatFile, () => {});
    } catch (e) {
        console.log("Could not save file: " + e);
    }


    res.end(); // let the client move on with life
});

// Receive POST request from client
// Returns all messages recieved after the specified time
router.post('/get', function (req, res) {

    // Expected format: milliseconds since 1970
    let fromTime = req.body;

    // Filter messages
    let response = { messages: []};
    chat.messages.forEach(message => {
        if (message.time > fromTime) {
            response.messages.push(message);
        }
    });

    res.send(JSON.stringify(response));
});





// Send old messages to client
router.get('/getFirst', function (req, res, next) {
    let messagesToSend;
    if (chat.messages.length < 10) {
        messagesToSend = 0;
    }
    else {
        messagesToSend = chat.messages.length - 10;
    }

    let tenLast = chat.messages.slice(messagesToSend);
    let messages = { messages: tenLast };
    let tosend = JSON.stringify(messages);

    console.log(tosend);
    res.send(tosend);
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