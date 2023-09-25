import { Types, Document } from "mongoose";

export interface ICurrency extends Document {
  _id: Types.ObjectId;
  CountryName: string;
  Symbol: string;
  Code: string;
  GoldPrice?: Number;
  Currency: string;
  Flag: string;
}

export type Code = Pick<ICurrency, "Code">;
