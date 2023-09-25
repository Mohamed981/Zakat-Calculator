import express from "express";
import cors from "cors";
import routes from "./routes";
import env from './utils/validateEnv';
import "dotenv/config";
import * as database from "./database";

const app = express();

app.use(cors());
app.use(express.json());
app.use("/api", routes);

async function initialize() {
  await database.connectDB();
  app.listen(env.PORT, () => {
    console.log("Server started on", env.PORT);
  });
}

initialize();