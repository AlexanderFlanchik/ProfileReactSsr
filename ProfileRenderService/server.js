const grpc = require('@grpc/grpc-js');
const express = require('express');
const path = require('path');
const router = require('./routes');
const { renderContent } = require('./services/GrpcRenderer.js');

const PROTO_PATH = "./proto/profile_service.proto";
const protoLoader = require('@grpc/proto-loader');
const options = { keepCase: true, longs: String, enums: String, defaults: true, oneofs: true };
const packageDefinition = protoLoader.loadSync(PROTO_PATH, options);
const profileServiceProto = grpc.loadPackageDefinition(packageDefinition);

// TODO: take this from ENV variables
const GRPC_HOST = "0.0.0.0";
const GRPC_PORT = 3002;
const EXPRESS_PORT = 3000;

// REST
const app = express();
app.use(express.json());
app.use('/content', express.static(path.join(__dirname, 'static')));

app.get("/", (_, res) => res.status(200).send("ProfileService is OK"));
app.use('/', router);

// GRPC
const server = new grpc.Server();

const pingHandler = (call, callback) => {
    const pingMessage = call.request.ping;
    console.log(pingMessage);

    callback(null, {
        pong: 'PONG!!!'
    });
}

server.addService(profileServiceProto.profile_service.v1.ProfileRenderService.service, { render: renderContent, ping: pingHandler });
server.bindAsync(`${GRPC_HOST}:${GRPC_PORT}`, grpc.ServerCredentials.createInsecure(), ( error, port) => {
    if (error) {
        console.log("Cannot set connection for GRPC server, exiting..");
        return;
    }

    server.start();
    console.log(`GRPC server started at ${port} port.`);
});

app.listen(EXPRESS_PORT, () => {
    console.log(`Express server started at ${EXPRESS_PORT} port.`);
});