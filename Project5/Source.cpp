#include <iostream>
#include <vector>
#include <cmath>
#include <algorithm>

const double PI = 3.141592653589793238463;

struct Point {
	long long xx, yy;
};

Point base;

double polar_angle(Point& base, Point& point)
{
	double alpha = atan2(point.yy - base.yy, point.xx - base.xx);
	if (alpha < 0) 
		alpha += 2 * PI;
	return alpha * (360 / (2 * PI));
}

int main() {
	std::vector<Point> polygon;
	std::vector<double> angles;
	int vertices;
	long long xx, yy;
	std::cin >> vertices;
	for (int i = 0; i < vertices; ++i) {
		std::cin >> xx >> yy;
		polygon.push_back({ xx, yy });
	}
	int min = 0;
	for (int i = 1; i < polygon.size(); ++i) {
		if (polygon[i].xx < polygon[min].xx || (polygon[i].xx == polygon[min].xx && polygon[i].yy < polygon[min].yy)) {
			min = i;
		}
	}
	base = polygon[min];

	for (int i = 0; i < polygon.size(); ++i) {
		angles[i] = polar_angle(base, polygon[i]);
	}

	std::sort(polygon.begin(), polygon.end(), [](Point first, Point second) -> bool {
		return polar_angle(base, first) < polar_angle(base, second);
	});
	std::sort(angles.begin(), angles.end());

	int queries;
	std::cin >> queries;
	for (int i = 0; i < queries; ++i) {
		std::cin >> xx >> yy;
		if (xx == base.xx && yy == base.yy) {
			std::cout << "BORDER\n";
		}
		Point query = { xx, yy };
		double angle = polar_angle(base, query);
		int place = upper_bound(angles.begin(), angles.end(), angle) - angles.begin();

	}
}
