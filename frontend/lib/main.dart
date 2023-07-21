import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import 'input_from.dart';

void main() {
  runApp(const MainApp());
}

class MainApp extends StatelessWidget {
  const MainApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        colorSchemeSeed: Colors.blueAccent,
        brightness: Brightness.light,
        useMaterial3: true,
        textTheme: TextTheme(
          headlineLarge: GoogleFonts.roboto(),
        ),
        appBarTheme: AppBarTheme(
          centerTitle: false,
          titleTextStyle: GoogleFonts.acme(
              color: Colors.black, letterSpacing: 4, fontSize: 20),
        ),
      ),
      home: InputFrom(),
    );
  }
}
