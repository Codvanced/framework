primeiroSetup = () -> 
	conta = [
		new Conta	'tvDth',			'25/11/14',	0		
		new Conta	'tvCabo',			'28/11/14',	0
		new Conta	'linhaFixa',		'12/11/14',	0
		new Conta	'internetFixa',		'12/11/14',	0
		new Conta	'internetCabo',		'04/11/14',	0
		new Conta	'tvDth',			'07/11/14',	0
		new Conta	'internetFibra',	'18/11/14',	0
		new Conta	'linhaMovel',		'10/11/14',	1
	]

	uni = new UnificaCalculo contas, '20/11/2014'
	uni.calculaVencimentos()

	$ '#tabela' 
		.prepend uni.getTables 'tab'
	@

segundoSetup = () -> 
	conta = [
		new Conta	'linhaFixa',	'01/11/14',	0
		new Conta	'tvCabo',		'07/11/14',	0
		new Conta	'internetFixa',	'01/11/14',	0
		new Conta	'linhaMovel',	'06/11/14',	1	
	]

	uni = new UnificaCalculo contas, '20/11/2014'
	uni.calculaVencimentos()

	$ '#tabela' 
		.prepend uni.getTable 'tab';
	@

terceiroSetup = () ->
	contas = [
		new Conta	'tvFibra',		'07/11/14',	0
		new Conta	'linhaFixa',	'15/11/14',	0
		new Conta	'internetFixa',	'15/11/14',	0
		new Conta	'linhaMovel',	'17/11/14',	1	
	];
			
	uni = new UnificaCalculo	contas,	'03/11/2014'	
	uni.calculaVencimentos()
		
	$ '#tabela'
		.prepend uni.getTable 'tab'
	@

addClick = (e) ->
	$data = $('#txtVencimento').val() 
	$prodId = $('#cmbProdutos :selected').val()
	$prodName = $('#cmbProdutos :selected').val()

	valid = false
	if $data is '' and $prodId is '' and valid = validaData($data,$prodId) is not ''
		alert valid
		return false
	
	$('selecionados tbody')
		.append '<tr>'+
			'<td value="'+$prodId+'">' + $prodName+'</td>'+
			'<td>'+$data+'</td>'+
			'<td>'+ (if $prodId is 'linhaMovel' then "<input type='checkbox'>" else "") +'</td>'+
			'<td><button class="btnRemove">Remover</button></td>'+
			'</tr>'
	@

$ () -> 
	primeiroSetup()
	segundoSetup()
	terceiroSetup()
	this