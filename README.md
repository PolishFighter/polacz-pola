![enter image description here](https://raw.githubusercontent.com/PolishFighter/polacz-pola/main/Logo.png)
## Zasady

Zadaniem gracza jest zaznaczenie wszystkich sąsiadujących pól o tym samym kolorze. Zaznaczone pola o tym samym kolorze znikają a    pozostałe spadają w dół. Nie da się zaznaczyć klocków o różnych    kolorach. W pojedynczym ruchu gracz może zaznaczyć od 4 do 12    sąsiadujących pól o tym samym kolorze. Zaznaczenie pól o różnych    kolorach oznacza stratę punktów. Sąsiadujące pola to takie które mają    wspólny bok. Zaznaczanie musi odbywać się w jednym ciągu, nie można    zaznaczyć pól które nie są bezpośrednimi sąsiadami. Nie można wracać    po już zaznaczonych polach. 
## Punktacja
### Zaznaczenie
Liczba punktów $p_{1}$ za zaznaczenie $n$ pól wynosi 
$$p_{1}=2^{n-4}$$ 
Zaznaczając 1 raz 8 pól, zdobywamy więcej punktów, niż zaznaczając 2 razy 4 pola.
$$2*2^{4-4} < 2^{8-4}$$
$$2 < 2^4$$
$$2 < 16$$
### Czas
Mając na dany poziom $t$ sekund i kończąc go w $e$ sekundzie punktację $p_{2}$ wyliczamy z wzoru
$$p_{2} = (1-(e/t)) * x$$
gdzie $x$ jest współczynikiem wzrostu punktów proporcjonalnie do pozostałego czasu (współczynik ten ustawiłem w grze na 200).
### Punktacja końcowa
Punktacje końcową $p$ wyliczamy z wzoru
$$p = p_{1} + p_{2}$$

Powtarzamy tą czyność dla każdego poziomu
## Inne
 - **Link do gry**: https://polishfighter.itch.io/pocz-pola
 - **Wersja**: Unity 2020.3.25f1

