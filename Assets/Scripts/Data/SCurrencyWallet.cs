using System.Collections.Generic;
using System.Text;

public struct SCurrencyWallet
{
    private Dictionary<ECurrency, int> _wallet;

    public int Copper
    {
        get { return _wallet[ECurrency.Copper]; }
    }

    public int Silver
    {
        get { return _wallet[ECurrency.Silver]; }
    }

    public int Gold
    {
        get { return _wallet[ECurrency.Gold]; }
    }

    public SCurrencyWallet(int startingCopper, int startingSilver = 0, int startingGold = 0)
    {
        _wallet = new Dictionary<ECurrency, int>()
                  {
                      { ECurrency.Copper, startingCopper },
                      { ECurrency.Silver, startingSilver },
                      { ECurrency.Gold, startingGold }
                  };
    }

    public override string ToString()
    {
        if (TotalValue(ECurrency.Copper).value == 0)
            return "0 Copper";

        StringBuilder sb             = new StringBuilder();
        bool          needsSeparator = false;

        if (Gold > 0)
        {
            sb.Append($"{Gold} Gold");
            needsSeparator = true;
        }

        if (Silver > 0)
        {
            if (needsSeparator)
                sb.Append(Gold > 0 && Copper == 0 ? " and " : ", ");

            sb.Append($"{Silver} Silver");
            needsSeparator = true;
        }

        if (Copper > 0)
        {
            if (needsSeparator)
                sb.Append(" and ");

            sb.Append($"{Copper} Copper");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Returns the value of the wallet, but instead of the raw currency values, they are summed
    /// and normalized into each denomination
    /// </summary>
    /// <example>234 Copper, will read as;<br/>"2 silver, 34 copper"</example>
    /// <returns></returns>
    public string ToStringNormalized()
    {
        int totalCopper = (int)TotalValue(ECurrency.Copper).value;
        if (totalCopper == 0)
            return "0 Copper";

        StringBuilder sb             = new StringBuilder();
        bool          needsSeparator = false;

        // Calculate normalized values
        int normalizedGold   = totalCopper / 10000;
        int remaining        = totalCopper % 10000;
        int normalizedSilver = remaining / 100;
        int normalizedCopper = remaining % 100;

        if (normalizedGold > 0)
        {
            sb.Append($"{normalizedGold} Gold");
            needsSeparator = true;
        }

        if (normalizedSilver > 0)
        {
            if (needsSeparator)
                sb.Append(normalizedGold > 0 && normalizedCopper == 0 ? " and " : ", ");

            sb.Append($"{normalizedSilver} Silver");
            needsSeparator = true;
        }

        if (normalizedCopper > 0)
        {
            if (needsSeparator)
                sb.Append(" and ");

            sb.Append($"{normalizedCopper} Copper");
        }

        return sb.ToString();
    }

    public void AddCurrency(SCurrencyWallet wallet)
    {
        AddCurrency(ECurrency.Copper, wallet.Copper);
        AddCurrency(ECurrency.Silver, wallet.Silver);
        AddCurrency(ECurrency.Gold, wallet.Gold);
    }

    /// <summary>
    /// Add currency of type <see cref="currency"/>. There is no caps or anything,
    /// as it is possible to have more than the required coins to be worth a Silver or gold, but
    /// it could still be the lower denomination (IE could have 200 coppers, and whilst that is worth
    /// 2 silver coins, you physically still have 200 coppers)
    ///
    /// Reason for this is when giving/receiving change, there might be a situation where neither you
    /// nor a vendor/customer has the change necessary. So have to compromise or lose a sale/purchase etc
    ///
    /// NOTE: It might be worth to simplify and WoW-ify it and just upgrade the currency automatically
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="amount"></param>
    public void AddCurrency(ECurrency currency, int amount)
    {
        _wallet[currency] += amount;
    }

    /// <summary>
    /// Get the total value of this wallet in one currency denomination
    /// Can be used to summarise total wealth easily as one number
    /// It is returned as a FLOAT because if you only have 1 copper then that
    /// is only 0.00001 Gold
    /// </summary>
    /// <param name="currency">Currency to display total wallet value as</param>
    /// <returns>Currency displayed as, as well as the total value in that currency</returns>
    /// <example>
    /// If total wallet is: 78 Copper, 128 Silver and 3 Gold then the results would be;
    ///     - Copper = 42,878
    ///     - Silver = 428.78
    ///     - Gold   = 4.2878
    /// It is possible to have more than the threshold of coins for that denomination, it doesn't
    /// automatically upgrade to that coin (you could have 100x 2p coins, but that doesn't become
    /// a £2 automatically)
    /// </example>
    public (ECurrency denomination, float value) TotalValue(ECurrency currency = ECurrency.Gold)
    {
        // 1 copper = lowest denomination
        // 1 silver = 100 coppers
        // 1 gold   = 100 silvers = 10,000 coppers
        switch (currency)
        {
            default:
            case ECurrency.Copper:
                int copperValue = _wallet[currency];
                copperValue += _wallet[ECurrency.Silver] * 100;
                copperValue += _wallet[ECurrency.Gold] * 10000;
                return (currency, copperValue);
            case ECurrency.Silver:
                float silverValue = _wallet[currency];
                silverValue += _wallet[ECurrency.Gold] * 100;
                silverValue += _wallet[ECurrency.Copper] / 100f;
                return (currency, silverValue);
            case ECurrency.Gold:
                float goldValue = _wallet[currency];
                goldValue += _wallet[ECurrency.Silver] / 100f;
                goldValue += _wallet[ECurrency.Copper] / 10000f;
                return (currency, goldValue);
        }
    }
}