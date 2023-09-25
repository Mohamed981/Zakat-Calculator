import Currency from "./models/currency.model";

export const find=(query,projections):Promise<Currency>=>{
    return Currency.find(query,projections);
}
