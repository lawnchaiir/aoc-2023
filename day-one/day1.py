import re

with open("input.txt") as f:
    lines = [l.strip() for l in f.readlines()]

def process_line(line):
    first_digit = None
    last_digit = None
    for offset in range(len(line)):
        if first_digit is None:
            front = line[offset]
            if front.isdigit():
                first_digit = front

        if last_digit is None:
            back = line[len(line) - 1 - offset]
            if back.isdigit():
                last_digit = back
        
        if first_digit is not None and last_digit is not None:
            calibration = int(first_digit + last_digit)
            return calibration

def solution_one():
    calibration_values = []
    for line in lines:
        calibration = process_line(line)
        calibration_values.append(calibration)
    print(sum(calibration_values))


solution_one()

numbers = {
    "one":"1",
    "two":"2",
    "three":"3",
    "four":"4",
    "five":"5",
    "six":"6",
    "seven":"7",
    "eight":"8",
    "nine":"9",
}

def replace_token(match):
    inner_match = match.groups(0)[0]
    return numbers[inner_match]

def solution_two():
    # need to do a lookahead assertion because there can be overlaps in the numbers
    # for example, eightwothree in the test input needs to provide an 823 sequence, not 8wo3
    # (the t is shared between eighT and Two). Because the lookahead discards the match context,
    # the .sub is doing inserts rather than replacements, so we get "8eigh2two3three"
    # which is fine since the numerical sequence is correct, though not necessarily ideal.
    number_replace_pattern =  "(?=(" + "|".join(numbers.keys()) + "))"
    number_replace_regex = re.compile(number_replace_pattern)

    calibration_values = []
    for line in lines:
        line = number_replace_regex.sub(replace_token, line)
        calibration = process_line(line)
        calibration_values.append(calibration)
    print(sum(calibration_values))


solution_two()