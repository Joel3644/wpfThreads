# wpfThreads
In precedenza abbiamo visto come si fa un'applicazione Wpf ora tentiamo di realizzare un'applicazione Wpf in cui abbiamo un pulsante che se clicchiamo deve iniziare farà iniziare a contare tre textbox la prima fino a 50 la seconda fino a 500 e la terza fino a 5000 con una quarta textbox che tenga conto del totale.
<br>
Intanto per realizzarlo creiamo un pulsante come abbiamo visto precedentemente.
Facciamo quindi una funzione che scriva i numeri da 0 a N e si attiva quando noi clicchiamo il pulsante.
<br>
```
private void incrementa(){
	for(int i = 0; x <= GIRI; x++){
		lblCounter.Text = x.ToString();  
	}
} //lblCounter rappresenta un textblock
```
