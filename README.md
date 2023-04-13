# wpfThreads
In precedenza abbiamo visto come si fa un'applicazione Wpf ora tentiamo di realizzare un'applicazione Wpf in cui abbiamo un pulsante che se clicchiamo deve iniziare farà iniziare a contare tre textbox la prima fino a 50 la seconda fino a 500 e la terza fino a 5000 con una quarta textbox che tenga conto del totale.

<br>

Intanto per realizzarlo creiamo un pulsante come abbiamo visto precedentemente.
Facciamo quindi una funzione che scriva i numeri da 0 a N e si attiva quando noi clicchiamo il pulsante.

```
private void incrementa(){
	for(int i = 0; x <= GIRI; x++){
		lblCounter.Text = x.ToString();  
	}
} //lblCounter rappresenta un textblock
```

Se facciamo però partire la nostra app così e clicchiamo il pulsante quello che succederà è che vedremo il numero N istantaneamente senza vederlo salire in tempo reale.

Di conseguenza quindi per vedere il numero effettivamente aumentare dobbiamo aggiungere un delay per aumentare il tempo che ci mette a raggiungere il numero massimo.
Per aggiungere un delay possiamo usare la funzione di sleep che è:

```
thread.sleep(1000);  	 //1 sec
```

Il problema ora è che quando premiamo il pulsante l’applicazione si bloccherà.
Questa funzione fa “addormentare” il thread in cui è eseguito il processo per 1 secondo, quindi ogni volta che aumenta il numero di 1 aspetta 1 secondo per continuare.
<br><br>
Anche così però non riusciamo a vedere il numero che si aumenta ma vedremo solamente il risultato finale di quando il thread ha finito di contare fino a N. 

<br><br>

Per evitare questo possiamo creare un nuovo thread e poi lo istanziamo e avviamo.

```
private void Button_Click(object sender, RoutedEventArgs e){
	Thread thread1 = new Thread(Incrementa1);
	thread.Start();
}
```
e aggiorniamo il metodo che chiamiamo IncrementCounter1 in questo modo:
```
private void incrementa1() {
    for (int x = 0; x < GIRI1; x++) {
	lock (_locker) {
	    _counter1++;
	    _counter4++;
	}

	Dispatcher.Invoke(() => {
	    lblCounter1.Text = _counter1.ToString();
	    lblCounter4.Text = _counter4.ToString();
	    prbarCounter1.Value = x;
	    prbarCounter4.Value = _counter4;
	});
	Thread.Sleep(100);
    }
    semaforo.Signal();	//segnala che il thread ha finito
}
```
Abbiamo usato Dispatcher.Invoke qui, sostanzialmente quello che succede è che il thread chiamante viene bloccato fino a quando l'azione programmata non viene completata.
<br>
Quindi possiamo utilizzarlo per eseguire un processo lento parallelamente a quello principale in modo da avere un aggiornamento visibile del nostro numero nella textbox.
<br><br>
Il counter4 rappresenta la somma tra tutti i thread che stanno eseguendo codice quindi sarà presente su tutti e tre i thread.
<br><br>
Faremo altri 2 metodi come quello di Incrementa1 che chiameremo Incrementa2 e Incrementa3 in cui faremo le stesse cose ma per le altre textbox.
<br><br>
Dovremo quindi modificare di conseguenza anche il metodo Button_Click facendo le seguenti modifiche.

```
private void Button_Click(object sender, RoutedEventArgs e) {
    btnGo.IsEnabled = false;     //disabilita il pulsante una volta premuto

    prbarCounter4.Maximum = (GIRI1 + GIRI2 + GIRI3);    //max counter4

    Thread thread1 = new(incrementa1);
    thread1.Start();

    Thread thread2 = new(incrementa2);
    thread2.Start();

    Thread thread3 = new(incrementa3);
    thread3.Start();

    semaforo = new CountdownEvent(3);    //semaforo che tiene conto dei processi

    Thread thread4 = new(() => {
	semaforo.Wait();
	Dispatcher.Invoke(() => {
	    lblCounter1.Text = _counter1.ToString();
	    lblCounter2.Text = _counter2.ToString();
	    lblCounter3.Text = _counter3.ToString();
	    lblCounter4.Text = _counter4.ToString();

	    prbarCounter1.Value = _counter1;
	    prbarCounter2.Value = _counter2;
	    prbarCounter3.Value = _counter3;
	    prbarCounter3.Value = _counter4;
	    btnGo.IsEnabled = true;   
	});
    });
    thread4.Start();
}
```
```
btnGo.IsEnabled = false;     
//disabilita il pulsante una volta premuto

btnGo.IsEnabled = true;	
//pulsante viene riattivato dato che i processi sono finiti su tutti i thread
```
Creiamo infine un pulsante per resettare i counter e le progress bar che aggiungiamo per vedere quanto manca a un processo per finire di calcolare.
```
private void btnClear_Click(object sender, RoutedEventArgs e) {
    _counter1 = 0;
    _counter2 = 0;
    _counter3 = 0;
    _counter4 = 0;
    lblCounter1.Text = _counter1.ToString();
    lblCounter2.Text = _counter2.ToString();
    lblCounter3.Text = _counter3.ToString();
    lblCounter4.Text = _counter4.ToString();

    prbarCounter1.Value = _counter1;
    prbarCounter2.Value = _counter2;
    prbarCounter3.Value = _counter3;
    prbarCounter4.Value = _counter4;
}
```
Ci ritroveremo quindi con questa situazione in conclusione:
<br>
![result](https://github.com/Joel3644/wpfThreads/blob/main/Img/WPFThreadResult.png?raw=true)
