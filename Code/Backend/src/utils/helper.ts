import { readFileSync } from "fs";
import { Constants, ICurrency } from "../../../shared";

export const Currencies = (): ICurrency[] => {
  return JSON.parse(readFileSync("./src/symbols.json").toString());
};

export const CountryCodesConcatenated = (): string => {
  const currencies: ICurrency[] = Currencies();
  let codes: string[] = [];
  codes = currencies.map((e) => e.Code);
  const codesConcatenated = codes.join(",").toString();
  return codesConcatenated;
};

export const ConvertCurrency = (c1: number, c2: number): number => {
  return +(c1 / c2).toFixed(2);
};

export const CheckZakat = async (wealth: number, standardValue: number = 0) => {
  let threshold: number = 0;
  if (standardValue !== 0) {
    threshold =
      (standardValue / Constants.OunceToGram) * Constants.GoldThreshold;
  } else threshold = Constants.GoldThreshold;
  if (threshold > wealth) return Constants.NOZAKAT;
  return wealth * Constants.Percentage;
};
