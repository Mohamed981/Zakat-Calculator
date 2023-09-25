/* eslint-disable @typescript-eslint/no-unused-vars */
import { Request, Response } from "express";
import Currency from "../models/currency.model";
import { config } from "dotenv";
import { Constants, ICurrency } from "../../../shared";
import {
  CountryCodesConcatenated,
  ConvertCurrency,
  CheckZakat,
  Currencies,
} from "../utils/helper";
import Str from "@supercharge/strings";

config();

const createCurrencies = async (req: Request, res: Response) => {
  const currencies: ICurrency[] = Currencies();
  Currency.create(currencies);

  res.send("DONE");
};

const createPrices = async (req: Request, res: Response) => {
  const codesConcatenated: string = CountryCodesConcatenated();
  const url = process.env.PRICES_URI + codesConcatenated;
  const result = await fetch(url).then((response) => response.json());
  const entries: [string, number][] = Object.entries(result.rates);

  entries.forEach(async (element) => {
    await Currency.findOneAndUpdate(
      { Code: element[0] },
      { GoldPrice: element[1] }
    );
  });

  res.send("DONE");
};

const CalculateWealth = async (
  standard: string,
  standardValue: number = 0,
  entries: [string, { Code: string; Value: number }][]
): Promise<number> => {
  let wealth: number = 0;
  const standards = entries.map((e) => e[1].Code);
  const values = await Currency.find(
    {
      Code: {
        $in: standards,
      },
    },
    { Code: 1, GoldPrice: 1 }
  );
  const map = new Map(
    values.map((obj) => {
      return [obj.Code, obj.GoldPrice];
    })
  );

  entries.forEach(async (e: [string, { Code: string; Value: number }]) => {
    if (e[0] !== standard) {
      const value: number = +map.get(e[1].Code);
      let c: number = 0;
      if (standard !== "XAU") c = e[1].Value * ConvertCurrency(standardValue, value);
      else c = (e[1].Value / value) * Constants.OunceToGram;
      wealth += c;
    } else wealth += e[1].Value;
  });
  return wealth;
};

const getZakat = async (req: Request, res: Response) => {
  const entries: [string, { Code: string; Value: number }][] = Object.entries(
    req.body.savings
  );
  const standard: string = req.body.standard;
  const standardValue: any | null = await Currency.findOne(
    { Code: standard },
    { GoldPrice: 1 }
  );

  const wealth = await CalculateWealth(
    standard,
    standardValue?.GoldPrice,
    entries
  );
  const Zakat = await CheckZakat(wealth, standardValue?.GoldPrice);
  res.send({
    status: "ok",
    message: Zakat,
  });
};

const getCurrencies = async (req: Request, res: Response) => {
  let currencies: ICurrency[] = await Currency.find({
    GoldPrice: { $ne: null },
  }).select("-Currency");
  res.send({ status: "ok", message: currencies });
};
export { createCurrencies, createPrices, getZakat, getCurrencies };
