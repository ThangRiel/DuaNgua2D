const express = require('express');
const cors = require('cors');
const app = express();

app.use(cors());
app.use(express.json());

// Lưu tạm % chạy của ngựa vào RAM
let horsesData = {};

// Unity bắn data vào đây
app.post('/api/update', (req, res) => {
    const { horseId, percent } = req.body;
    horsesData[horseId] = percent;
    res.sendStatus(200);
});

// Giao diện web lấy data từ đây
app.get('/api/data', (req, res) => {
    res.json(horsesData);
});

// Trả về file HTML
app.get('/', (req, res) => {
    res.sendFile(__dirname + '/index.html');
});

app.listen(8080, () => console.log('Server đang chạy tại: http://localhost:8080'));