﻿namespace Ordering.Domain.ValueObjects;

public record Address
{
	public string FirstName { get; } = default!;
	public string LastName { get; } = default!;
	public string? EmailAddress { get; } = default!;
	public string AddressLine { get; } = default!;
	public string Country { get; } = default!;
	public string State { get; } = default!;
	public string ZipCode { get; } = default!;
	protected Address() // used for inheritance (protected)
	{
	}
	private Address(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
	{
		//منع إنشاء object بـ new Address(...) مباشرة.
		//بيستخدم فقط داخليًا جوة الكلاس نفسه(زي لما تستعمله جوة Of).

		FirstName = firstName;
		LastName = lastName;
		EmailAddress = emailAddress;
		AddressLine = addressLine;
		Country = country;
		State = state;
		ZipCode = zipCode;
	}

	public static Address Of(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);
		ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);

		return new Address(firstName, lastName, emailAddress, addressLine, country, state, zipCode);
	}
}
