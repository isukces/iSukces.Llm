namespace Demo.Structural;

internal sealed class SampleData
{

    /// <summary>
    /// Przepis z https://www.przepisy.pl/przepis/meksykanskie-krokiety-z-wolowina
    /// </summary>
    public static string Krokiety =
        """
        Meksykańskie krokiety z wołowiną
        NOWY PRZEPIS
        Autor: przepisy.pl
        ratingratingratingratingrating
        89

        heart
        2705 os.
        cooked
        stopień trudności
        łatwe

        czas przygotowania
        70 min.
        ilość porcji
        4 os.
        Drukuj 1209 osób wydrukowało
        Pobierz pdf3126 osób zapisało
        Udostępnij
        Meksykańskie krokiety z wołowiną
        53
        Smakowało? Podziel się opinią
        Składniki
        Szponder wołowy

        350 gramów

        włoszczyzna

        1 pęczek

        czarna fasola z puszki

        150 gramów

        papryka czerwona

        1 sztuka

        papryka żółta

        1 sztuka

        Przyprawa Adobo Knorr

        2 łyżki

        kolendra

        1 pęczek

        limonka

        1 sztuka

        olej do smażenia

        50 mililitrów

        sos beszamel:

        masło

        2 łyżki

        mąka pszenna

        2 łyżki

        mleko

        250 mililitrów

        do panierowania:

        jajka

        2 sztuki

        mąka

        4 łyżki

        bułka tarta

        1 szklanka

        Email Listonic
        Przygotowanie krok po kroku
        Krok 1
        Z wołowiny wraz z włoszczyzną i kostką Knorr ugotuj rosół. Jak mięso będzie miękkie, odcedź rosół i zachowaj do innej potrawy. Mięso ostudź i następnie pokrój na drobne kawałki.

        Meksykańskie krokiety z wołowiną - Krok 1
        Krok 2
        W tym czasie przygotuj sos beszamel. Rozpuść na patelni masło, dodaj mąkę i jak się połączą wlej mleko. Wymieszaj dokładnie i zagotuj. W razie potrzeby dopraw szczyptą soli.

        Meksykańskie krokiety z wołowiną - Krok 2
        Krok 3
        Odcedź fasolę z puszki. Papryki pokrój w drobną kotkę i podsmaż na patelni. Dodaj pokrojone mięso wołowe, fasolę, przyprawę Adobo Knorr i smaż całość kilka minut aż składniki się połączą.

        Meksykańskie krokiety z wołowiną - Krok 3
        Krok 4
        Kolendrę posiekaj, wyciśnij sok z limonki. Teraz w dużej misce wymieszaj mięso wołowe, beszamel, kolendrę i sok z limonki. Całość powinna być dość gęsta i kleista. Przełóż wszystko do płaskiego naczynia i włóż do lodówki aż dobrze wystygnie i stężeje.

        Meksykańskie krokiety z wołowiną - Krok 4
        Krok 5
        Przygotuj składniki do panierowania. Rozkłóć jajko, wysyp na talerze mąkę i bułkę tartą. W dłoniach formuj małe krokiety. Panieruj klasycznie w mące, jajku i bułce tartej. Tak przygotowane krokiety możesz smażyć od razu w oleju lub zamrozić i smażyć w dowolnym momencie.
        """;

    public static readonly string SystemPrompt =
        """
            Jesteś inteligentnym asystentem o nazwie Bielik. Wykonujesz instrukcje użytkownika, zgodnie z jego poleceniami. Stosujesz się do formatu odpowiedzi (schematu JSON) narzuconego przez użytkownika. Nie odpowiadasz ŻADNYM dodatkowym tekstem. 
            Użytkownik wysyła tekst do przetworzenia (przepis kulinarny), a Ty ekstrachujesz z niego informacje zgodnie ze schematem.
            """.Replace("\r\n", "\n").Replace('\n', ' ').Trim();
}
