public class Move {

    public Hex hex;
    public int price;

    public Move(Hex _hex, int _price)
    {
        if (_hex != null)
        {
            hex = _hex;
        }

        if (_price >= 0)
        {
            price = _price;
        }
    }
}
