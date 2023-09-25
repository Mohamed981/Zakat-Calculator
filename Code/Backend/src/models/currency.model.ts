import mongoose from "mongoose";
import { ICurrency } from "../../../shared";

const Currency = new mongoose.Schema<ICurrency>(
  {
    CountryName:{type:String},
    Symbol: { type: String },
    Code:{type:String},
    GoldPrice:{type:Number},
    Currency:{type:String},
    Flag:{type:String},
  },
  { collection: "currency" }
);

const model = mongoose.model<ICurrency>("Currency", Currency);

export default model;