import * as currencyController from "./controllers/currency.controller";
import express from "express";

const router = express.Router();

router.post("/currencies", currencyController.createCurrencies);
router.post("/prices", currencyController.createPrices);
router.post("/zakat", currencyController.getZakat);
router.get("/currencies", currencyController.getCurrencies);

export default router;
