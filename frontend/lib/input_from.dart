//In the name of Allah

import 'package:dio/dio.dart';
import 'package:dropdown_search/dropdown_search.dart';
import 'package:flutter/material.dart';

final ValueNotifier<List<UserModel>> selectedUsers =
    ValueNotifier<List<UserModel>>([]);

// ignore: must_be_immutable
class InputFrom extends StatelessWidget {
  InputFrom({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: PreferredSize(
        preferredSize: const Size.fromHeight(50.0),
        child: AppBar(
          title: const Text("Input From"),
        ),
      ),
      body: Column(
        children: [
          const Divider(
            color: Colors.blueAccent,
            height: 2,
            thickness: 2,
            indent: 20,
            endIndent: 20,
          ),
          const SizedBox(height: 20),
          Padding(
            padding: const EdgeInsets.only(left: 50, right: 50),
            child: Column(
              children: [
                FirstRow(),
                const SizedBox(height: 10),
                SecondRow(),
                const SizedBox(height: 10),
                ThirdRow(),
                //start
                SizedBox(
                  height: 200,
                  child: Expanded(
                    flex: 1,
                    child: ValueListenableBuilder<List<UserModel>>(
                      valueListenable: selectedUsers,
                      builder: (context, value, child) {
                        return ListView.builder(
                          itemCount: value.length,
                          itemBuilder: (context, index) {
                            final user = value[index];
                            return ListTile(
                              leading: CircleAvatar(
                                backgroundImage: NetworkImage(user.avatar),
                              ),
                              title: Text(user.name),
                              subtitle: Text(user.createdAt.toIso8601String()),
                            );
                          },
                        );
                      },
                    ),
                  ),
                ),
                const SizedBox(height: 20),
                FivethRow(),
              ],
            ),
          ),
        ],
      ),
    );
  } //build
} //class

class FirstRow extends StatelessWidget {
  const FirstRow({super.key});

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        const SizedBox(
          width: 250,
          height: 40,
          child: TextField(
            decoration: InputDecoration(
              border: OutlineInputBorder(),
              labelText: 'Bill No',
            ),
          ),
        ),
        const SizedBox(width: 8.0),
        SizedBox(
          height: 40,
          child: ElevatedButton(
            onPressed: () {
              // handle find bill
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.blueAccent,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(5),
              ),
            ),
            child: const Text(
              'Find',
              style: TextStyle(color: Colors.white),
            ),
          ),
        ),
      ],
    );
  }
} //class

class SecondRow extends StatelessWidget {
  SecondRow({super.key});

  DateTime date = DateTime.now();

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: 60,
      child: Row(
        children: [
          Expanded(
            flex: 10,
            child: DropdownSearch<UserModel>(
              popupProps: const PopupProps.menu(
                showSearchBox: true,
                fit: FlexFit.loose,
              ),
              asyncItems: (String filter) async {
                var response = await Dio().get(
                  "http://5d85ccfb1e61af001471bf60.mockapi.io/user",
                  queryParameters: {"filter": filter},
                );
                var models = UserModel.fromJsonList(response.data);
                return models;
              },
              itemAsString: (UserModel data) => data.name,
              dropdownDecoratorProps: const DropDownDecoratorProps(
                dropdownSearchDecoration: InputDecoration(
                  labelText: "Products",
                  border: OutlineInputBorder(),
                  alignLabelWithHint: false,
                ),
              ),
              onChanged: (UserModel? data) {
                if (data != null) {
                  selectedUsers.value = [...selectedUsers.value, data];
                }
              },
            ),
          ),
          const SizedBox(width: 10),
          Expanded(
            flex: 6,
            child: DropdownSearch<UserModel>(
              popupProps: const PopupProps.menu(
                showSearchBox: true,
                fit: FlexFit.loose,
              ),
              asyncItems: (String filter) async {
                var response = await Dio().get(
                  "http://5d85ccfb1e61af001471bf60.mockapi.io/user",
                  queryParameters: {"filter": filter},
                );
                var models = UserModel.fromJsonList(response.data);
                return models;
              },
              itemAsString: (UserModel data) => data.name,
              dropdownDecoratorProps: const DropDownDecoratorProps(
                dropdownSearchDecoration: InputDecoration(
                  labelText: "Customers",
                  border: OutlineInputBorder(),
                  alignLabelWithHint: false,
                ),
              ),
              onChanged: (UserModel? data) {
                print(data);
              },
            ),
          ),
          const SizedBox(width: 10),
          SizedBox(
            height: 50,
            width: 100,
            child: ElevatedButton(
              onPressed: () async {
                final pickedDate = await showDatePicker(
                  context: context,
                  initialDate: date,
                  firstDate: DateTime(2015, 8),
                  lastDate: DateTime(2101),
                );
                if (pickedDate != null && pickedDate != date) {}
              },
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.blueAccent,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(5),
                ),
              ),
              child: const Text(
                "Date",
                style: TextStyle(color: Colors.white),
              ),
            ),
          ),
        ],
      ),
    );
  }
} //class

class UserModel {
  final String id;
  final DateTime createdAt;
  final String name;
  final String avatar;

  UserModel({
    required this.id,
    required this.createdAt,
    required this.name,
    required this.avatar,
  });

  factory UserModel.fromJson(Map<String, dynamic> json) {
    return UserModel(
      id: json["id"],
      createdAt: DateTime.parse(json["createdAt"]),
      name: json["name"],
      avatar: json["avatar"],
    );
  }

  static List<UserModel> fromJsonList(List list) {
    return list.map((item) => UserModel.fromJson(item)).toList();
  }

  ///this method will prevent the override of toString
  String userAsString() {
    return '#${this.id} ${this.name}';
  }

  ///this method will prevent the override of toString
  bool userFilterByCreationDate(String filter) {
    return this.createdAt.toString().contains(filter);
  }

  ///custom comparing function to check if two users are equal
  bool isEqual(UserModel model) {
    return this.id == model.id;
  }

  @override
  String toString() => name;
}

class ThirdRow extends StatelessWidget {
  const ThirdRow({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      height: 40,
      color: Colors.blue,
      child: const Row(
        children: [
          Expanded(
            flex: 3,
            child: Padding(
              padding: EdgeInsets.all(8.0),
              child: Text("Products", overflow: TextOverflow.ellipsis),
            ),
          ),
          VerticalDivider(
            color: Colors.red,
            thickness: 2,
            indent: 10,
            endIndent: 10,
          ),
          Expanded(
            flex: 1,
            child: Padding(
              padding: EdgeInsets.all(8.0),
              child: Text("Rate", overflow: TextOverflow.ellipsis),
            ),
          ),
          VerticalDivider(
            color: Colors.red,
            thickness: 2,
            indent: 10,
            endIndent: 10,
          ),
          Expanded(
            flex: 1,
            child: Padding(
              padding: EdgeInsets.all(8.0),
              child: Text("Qty", overflow: TextOverflow.ellipsis),
            ),
          ),
          VerticalDivider(
            color: Colors.red,
            thickness: 2,
            indent: 10,
            endIndent: 10,
          ),
          Expanded(
            flex: 2,
            child: Padding(
              padding: EdgeInsets.all(8.0),
              child: Text("Total Amount", overflow: TextOverflow.ellipsis),
            ),
          ),
          VerticalDivider(
            color: Colors.red,
            thickness: 2,
            indent: 10,
            endIndent: 10,
          ),
          Expanded(
            flex: 2,
            child: Padding(
              padding: EdgeInsets.all(8.0),
              child: Text("Discount(Amt)", overflow: TextOverflow.ellipsis),
            ),
          ),
          VerticalDivider(
            color: Colors.red,
            thickness: 2,
            indent: 10,
            endIndent: 10,
          ),
          Expanded(
            flex: 2,
            child: Padding(
              padding: EdgeInsets.all(8.0),
              child: Text("Net Amount", overflow: TextOverflow.ellipsis),
            ),
          ),
        ],
      ),
    );
  } //build
} //class

class FivethRow extends StatelessWidget {
  const FivethRow({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          SizedBox(
            height: 35,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: [
                Padding(
                  padding: EdgeInsets.all(8.0),
                  child: Text("Net Total", overflow: TextOverflow.ellipsis),
                ),
                Container(
                  width: 100,
                  height: 50,
                  decoration: BoxDecoration(
                    border: Border.all(
                      color: Colors.blue,
                      width: 2,
                    ),
                  ),
                  child: Center(
                      child:
                          Text("000000000", overflow: TextOverflow.ellipsis)),
                ),
              ],
            ),
          ),
          SizedBox(
            height: 35,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: [
                Padding(
                  padding: EdgeInsets.all(8.0),
                  child:
                      Text("Discount Total", overflow: TextOverflow.ellipsis),
                ),
                Container(
                  width: 100,
                  height: 50,
                  decoration: BoxDecoration(
                    border: Border.all(
                      color: Colors.blue,
                      width: 2,
                    ),
                  ),
                  child: Center(
                      child:
                          Text("000000000", overflow: TextOverflow.ellipsis)),
                ),
              ],
            ),
          ),
          SizedBox(
            height: 35,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: [
                Padding(
                  padding: EdgeInsets.all(8.0),
                  child: Text("Paid Total", overflow: TextOverflow.ellipsis),
                ),
                Container(
                  width: 100,
                  height: 50,
                  decoration: BoxDecoration(
                    border: Border.all(
                      color: Colors.blue,
                      width: 2,
                    ),
                  ),
                  child: Center(
                      child:
                          Text("000000000", overflow: TextOverflow.ellipsis)),
                ),
              ],
            ),
          ),
          SizedBox(
            height: 35,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: [
                Padding(
                  padding: EdgeInsets.all(8.0),
                  child: Text("Due Total", overflow: TextOverflow.ellipsis),
                ),
                Container(
                  width: 100,
                  height: 35,
                  decoration: BoxDecoration(
                    border: Border.all(
                      color: Colors.blue,
                      width: 2,
                    ),
                  ),
                  child: Center(
                      child:
                          Text("000000000", overflow: TextOverflow.ellipsis)),
                ),
              ],
            ),
          ),
          SizedBox(
            height: 35,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: [
                ElevatedButton(
                  onPressed: () {
                    // handle find bill
                  },
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.blueAccent,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(5),
                    ),
                  ),
                  child: const Text(
                    'Save Changes',
                    style: TextStyle(color: Colors.white),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  } //build
} //class
